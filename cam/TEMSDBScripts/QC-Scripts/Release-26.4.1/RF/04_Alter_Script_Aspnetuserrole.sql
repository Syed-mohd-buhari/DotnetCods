DELETE FROM AspNetUserRoles
WHERE ROWID NOT IN (
    SELECT MIN(ROWID)
    FROM AspNetUserRoles
    GROUP BY UserId, RoleId
);
commit;


delete from gridcustomcolumn where classname = 'AspnetuserGridDto';