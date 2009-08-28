using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace LookupUserBySid
{
    class Program
    {
        public enum SID_NAME_USE
        {
            User = 1,
            Group,
            Domain,
            Alias,
            WellKnownGroup,
            DeletedAccount,
            Invalid,
            Unknown,
            Computer
        }

        /// <summary>
        /// The ConvertStringSidToSid function converts a string-format security identifier (SID) into a valid,
        /// functional SID. You can use this function to retrieve a SID that the ConvertSidToStringSid function
        /// converted to string format.
        /// </summary>
        /// <param name="Text">Pointer to a null-terminated string containing the string-format SID to convert. The SID
        /// string can use either the standard S-R-I-S-S... format for SID strings, or the SID string constant format,
        /// such as "BA" for built-in administrators.</param>
        /// <param name="SId">Pointer to a variable that receives a pointer to the converted SID. To free the returned
        /// buffer, call the LocalFree function.</param>
        /// <returns>Returns <c>true</c> for success, otherwise <c>false</c>.</returns>
        [DllImport("AdvAPI32.dll")]
        public static extern bool ConvertStringSidToSid(string Text, ref IntPtr Sid);

        /// <summary>
        /// The LookupAccountSid function accepts a security identifier (SID) as input. It retrieves the name of the
        /// account for this SID and the name of the first domain on which this SID is found.
        /// </summary>
        /// <param name="SystemName">Pointer to a null-terminated character string that specifies the target computer.
        /// This string can be the name of a remote computer. If this string is NULL, the account name translation
        /// begins on the local system. If the name cannot be resolved on the local system, this function will try to
        /// resolve the name using domain controllers trusted by the local system. Generally, specify a value for
        /// SystemName only when the account is in an untrusted domain and the name of a computer in that domain is
        /// known.</param>
        /// <param name="SId">Pointer to the SID to look up.</param>
        /// <param name="Name">Pointer to a buffer that receives a null-terminated string that contains the account name
        /// that corresponds to the <paramref name="SId"/> parameter.</param>
        /// <param name="NameBytes">On input, specifies the size, in TCHARs, of the Name buffer. If the function fails
        /// because the buffer is too small or if NameBytes is zero, NameBytes receives the required buffer size,
        /// including the terminating null character.</param>
        /// <param name="DomainName">Pointer to a buffer that receives a null-terminated string that contains the name
        /// of the domain where the account name was found.</param>
        /// <param name="DomainNameBytes">On input, specifies the size, in TCHARs, of the DomainName buffer. If the
        /// function fails because the buffer is too small or if DomainNameBytes is zero, DomainNameBytes receives the
        /// required buffer size, including the terminating null character.</param>
        /// <param name="NameUse">Pointer to a variable that receives a <see cref="SID_NAME_USE"/> value that indicates
        /// the type of the account.</param>
        /// <returns>Returns <c>true</c> for success, otherwise <c>false</c>.</returns>
        [DllImport("AdvAPI32.dll")]
        public static extern bool LookupAccountSid(string SystemName, IntPtr Sid, StringBuilder Name, ref int NameBytes, StringBuilder DomainName, ref int DomainNameBytes, ref SID_NAME_USE NameUse);

        /// <summary>
        /// The LookupAccountName function accepts the name of a system and an account as input. It retrieves a security
        /// identifier (SID) for the account and the name of the domain on which the account was found.
        /// </summary>
        /// <param name="SystemName">Pointer to a null-terminated character string that specifies the name of the
        /// system. This string can be the name of a remote computer. If this string is NULL, the account name
        /// translation begins on the local system. If the name cannot be resolved on the local system, this function
        /// will try to resolve the name using domain controllers trusted by the local system. Generally, specify a
        /// value for SystemName only when the account is in an untrusted domain and the name of a computer in that
        /// domain is known.</param>
        /// <param name="AccountName">Pointer to a null-terminated string that specifies the account name.</param>
        /// <param name="Sid">Pointer to a buffer that receives the SID structure that corresponds to the account name
        /// pointed to by the AccountName parameter. If this parameter is NULL, SidSize must be zero.</param>
        /// <param name="SidSize">Pointer to a variable. On input, this value specifies the size, in bytes, of the Sid
        /// buffer. If the function fails because the buffer is too small or if SidSize is zero, this variable receives
        /// the required buffer size.</param>
        /// <param name="DomainName">Pointer to a buffer that receives the name of the domain where the account name is
        /// found. For computers that are not joined to a domain, this buffer receives the computer name. If this
        /// parameter is NULL, the function returns the required buffer size.</param>
        /// <param name="DomainNameSize">Pointer to a variable. On input, this value specifies the size, in TCHARs, of
        /// the DomainName buffer. If the function fails because the buffer is too small, this variable receives the
        /// required buffer size, including the terminating null character. If the DomainName parameter is NULL, this
        /// parameter must be zero.</param>
        /// <param name="NameUse">Pointer to a SID_NAME_USE enumerated type that indicates the type of the account when
        /// the function returns.</param>
        /// <returns>Returns <c>true</c> for success, otherwise <c>false</c>.</returns>
        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern int LookupAccountName(
            string ServerName,
            string AccountName,
            IntPtr Sid,
            ref int SidSize,
            StringBuilder DomainName,
            ref int DomainNameSize,
            ref SID_NAME_USE SidUse);

        [DllImport("Advapi32.dll")]
        public static extern bool ConvertSidToStringSid(IntPtr Sid, ref StringBuilder StringSid);

        public static string SidToName(string strSid)
        {
            IntPtr Sid = IntPtr.Zero;
            bool ret = ConvertStringSidToSid(strSid, ref Sid);
            string accountName = string.Empty;

            if (ret)
            {
                StringBuilder Name = new StringBuilder(256);
                int NameBytes = 256;
                StringBuilder Domain = new StringBuilder(256);
                int DomainBytes = 256;
                SID_NAME_USE NameUse = SID_NAME_USE.Unknown;

                ret = LookupAccountSid(null, Sid, Name, ref NameBytes, Domain, ref DomainBytes, ref NameUse);

                if (ret)
                {
                    accountName = string.Format("{0}\\{1}", Domain, Name);
                }
            }

            Marshal.FreeHGlobal(Sid);

            return accountName;
        }

        public static string NameToSid(string strAccountName)
        {
            bool bRet = false;
            int lSidSize = 256;
            int lDomainNameSize = 256;

            string accountSid = "";
            string strDomainName = "";
            SID_NAME_USE AccountType = SID_NAME_USE.Unknown;
            StringBuilder strName;
            lSidSize = 0;
            IntPtr Sid = IntPtr.Zero;

            // First get the required buffer sizes for SID and domain name.
            int nRet = LookupAccountName(
                                null,
                                strAccountName,
                                Sid,
                                ref lSidSize,
                                null,
                                ref lDomainNameSize,
                                ref AccountType);
            bRet = (0 != nRet);
            if (!bRet)
            {
                int nErr = Marshal.GetLastWin32Error();
                if (122 == nErr) // Buffer too small
                {
                    // Allocate the buffers with actual sizes that are required
                    // for SID and domain name.
                    strName = new StringBuilder(lDomainNameSize);
                    Sid = Marshal.AllocHGlobal(lSidSize);
                    nRet = LookupAccountName(
                        null,
                        strAccountName,
                        Sid,
                        ref lSidSize,
                        strName,
                        ref lDomainNameSize,
                        ref AccountType);
                    bRet = (0 != nRet);
                    if (bRet)
                    {
                        StringBuilder strSid = new StringBuilder(256);
                        ConvertSidToStringSid(Sid, ref strSid);

                        strDomainName = strName.ToString();
                        accountSid = strSid.ToString();
                    }
                }
                else
                {
                    Console.WriteLine(nErr);
                }
            }
            Marshal.FreeHGlobal(Sid);
            return accountSid;
        }

        static void Main(string[] args)
        {
            string sid = args[0];

            Console.WriteLine(sid);

            string user = SidToName(sid);

            Console.WriteLine(user);

            sid = NameToSid(user);

            Console.WriteLine(sid);

            user = SidToName(sid);

            Console.WriteLine(user);
        }
    }
}
