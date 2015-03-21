# Introduction #

How-To build an Ubuntu Firewall

WARNING: All of this should be done behind an existing firewall.

DO NOT HOOK UP THIS SERVER TO THE INTERNET UNTIL YOUR FIREWALL IS IN PLACE.

## Install Ubuntu Server ##

Don't select any packages during the install, as we will install only what we need.

Format your hard drive as follows:
  * swap - 512MB
  * /boot - 128MB
  * / - 8GB
  * /tmp - 8GB
  * /home - 8GB
  * /var - Everything Else (This is where logs and data go.)

/tmp and /var should be marked nosuid and noexec for security.

[Automatic Updates](https://help.ubuntu.com/10.04/serverguide/C/automatic-updates.html)

## Software Needed ##

  1. shorewall6
  1. dnsmasq
  1. squid3
  1. noip2
  1. ssmtp
  1. moblock
  1. havp
  1. dansguardian
  1. sarg
  1. clamav-freshclam
  1. snort
  1. ntp
  1. pptpd
  1. openvpn
  1. logrotate
  1. samba
  1. upnp

## Update ##

sudo apt-get update
sudo apt-get upgrade

sudo reboot now

## Setup networking ##

sudo nano /etc/udev/rules.d/set-interface-names.rules

```
KERNEL=="eth?", SYSFS{address}=="00:30:18:a8:0b:6a", NAME="eth0"
KERNEL=="eth?", SYSFS{address}=="00:30:18:a6:2b:12", NAME="eth1"
KERNEL=="eth?", SYSFS{address}=="00:30:18:a6:2b:13", NAME="eth2"
KERNEL=="eth?", SYSFS{address}=="00:30:18:a6:2b:14", NAME="eth3"
```

sudo nano /etc/network/interfaces
```
# The loopback network interface
auto lo
iface lo inet loopback

# The primary network interface
auto eth0
iface eth0 inet dhcp

auto eth1
iface eth1 inet static
address 192.168.0.1
netmask 255.255.255.0
```

## Install ##

sudo apt-get install shorewall6 logrotate ntp openssh-server dnsmasq

## dnsmasq ##

sudo nano /etc/dnsmasq.conf

```
interface=eth1
expand-hosts
domain=rickbassham.com
dhcp-range=192.168.0.50,192.168.0.100,1h

# Set the NTP time server address to be the same machine as
# is running dnsmasq
dhcp-option=42,0.0.0.0
```

sudo /etc/init.d/dnsmasq restart

## Shorewall ##

sudo nano /etc/shorewall/shorewall.conf

```
SUBSYSLOCK=/var/run
IP_FORWARDING=On
STARTUP_ENABLED=Yes
```

cd /usr/share/doc/shorewall/examples/two-interfaces

cp **/etc/shorewall/**

[Two-Interface Setup](http://www.shorewall.net/two-interface.htm)

sudo nano /etc/shorewall/interfaces

```
#ZONE	INTERFACE	BROADCAST	OPTIONS
net		eth0		detect		dhcp tcpflags
loc		eth1		detect		dhcp nosmurfs tcpflags
```

sudo nano /etc/shorewall/rules
```
#ACTION			SOURCE	DEST			PROTO		DEST		SOURCE  	ORIGINAL
#													PORT		PORT(S)		DEST
SSH(ACCEPT)		loc		$FW
NTP(ACCEPT)		loc		$FW
DNS(ACCEPT)		loc		$FW

DNS(ACCEPT)		$FW		net

DNAT			net		loc:192.168.0.5	tcp			20883
```

sudo nano /etc/shorewall/policy
```
#SOURCE         DEST            POLICY          LOG           BURST:LIMIT
#                                               LEVEL
loc             net             ACCEPT
net             all             DROP            info
#
# THE FOLLOWING POLICY MUST BE LAST
#
all             all             REJECT          info

```

sudo /etc/init.d/shorewall restart

At this point, your firewall is in place. You can now hook up this machine to the internet. (But test for possible intrusion points first.)


## TODO ##
squid3 noip2 bind9 bind9utils ssmtp havp dansguardian sarg clamav-freshclam snort snort-rules-default linux-igd