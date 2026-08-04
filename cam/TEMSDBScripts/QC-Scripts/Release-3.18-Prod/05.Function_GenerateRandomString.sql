create or replace NONEDITIONABLE function GenerateRandomString(p_Characters varchar2, p_length number)
    return varchar2
    is
    l_res varchar2(256);
    begin
    select substr(listagg(substr(p_Characters, level, 1)) within group(order by dbms_random.value), 1, p_length)
    into l_res from dual
    connect by level <= length(p_Characters);
    return l_res;
end;