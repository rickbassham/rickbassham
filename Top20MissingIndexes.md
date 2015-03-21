
```
SELECT TOP 20 [statement], [equality_columns], [inequality_columns], [included_columns], 
avg_total_user_cost, avg_user_impact, user_seeks, user_scans
FROM sys.dm_db_missing_index_group_stats migs
INNER JOIN sys.dm_db_missing_index_groups AS mig
    ON (migs.group_handle = mig.index_group_handle)
INNER JOIN sys.dm_db_missing_index_details AS mid
    ON (mig.index_handle = mid.index_handle)

ORDER BY avg_total_user_cost * avg_user_impact * (user_seeks + user_scans) DESC;
```