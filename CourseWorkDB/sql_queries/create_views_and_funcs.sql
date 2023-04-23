-- Эти запросы запускать поочередно

create view salesView as select S.id, time "datetime", salesmanId, SM.name "manName", SC.price "summa"
from Sales S left join SalesContent SC 
on S.id = SC.id join Salesmen SM on S.salesmanId = SM.id ;

create function getSalesContent(@id int)
returns table 
as return (select G.name "name", SC.price, SC.amount, price*amount as "sum"
from Goods G left join SalesContent SC on G.id = SC.goodId where SC.saleId= @id);

