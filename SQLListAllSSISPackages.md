```sql

Declare @destServer sysname

Select @destServer = 'SERVER'

with folders (folderid, parentfolderid, foldername, path)
as
(
select folderid, parentfolderid, foldername, convert(varchar(100), '') as path
from msdb.dbo.sysdtspackagefolders90
where parentfolderid is null

union all

select r1.folderid, r1.parentfolderid, r1.foldername,
case when datalength(r2.path) > 0 then convert(varchar(100), r2.path + '\' + cast(r1.foldername as varchar(100)))
else convert(varchar(100), cast(r1.foldername as varchar(100)))
end as path
from msdb.dbo.sysdtspackagefolders90 as r1
join folders as r2 on r1.parentfolderid = r2.folderid
)
select
'dtutil /SourceServer ' + (Select name from sys.servers where server_id = 0) + ' /SQL "' +
case when datalength(folders.path) > 0 then '\' + folders.path + '\' + p.name
else '\' + p.name
end + '" /Encrypt FILE;"' + p.name + '.dtsx";2;test'
as export,

'dtutil /FILE "' +
p.name
+ '.dtsx" /Dec test /COPY SQL;"' + case when datalength(folders.path) > 0 then '\' + folders.path + '\' + p.name
else '\' + p.name
end + '" /QUIET /DestServer ' + @destServer
as import,

case when datalength(folders.path) > 0 then '\' + folders.path + '\' + p.name
else '\' + p.name
end as package,
p.name
from msdb.dbo.sysdtspackages90 p
inner join folders on folders.folderid = p.folderid
order by path
```