/*
	Adding more teams 
	Change date() to GetDate() for SQL server
*/

insert into teams(name, CreatedDate, createdby) 
values 
('Chelsea FC',GetDate(),'TestUser1'),
('Real Madrid',GetDate(),'TestUser1'),
('Benfica',GetDate(),'TestUser1'),
('Inter Milan',GetDate(),'TestUser1'),
('Inter Miami',GetDate(),'TestUser1'),
('Seba United',GetDate(),'TestUser1')