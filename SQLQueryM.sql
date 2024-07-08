CREATE TABLE [dbo].[Users] (
    [id_user]        INT        IDENTITY (1, 1) NOT NULL,
    [Login_user]    VARCHAR(50)    NOT NULL,
    [password_user]    VARCHAR(50)    NOT NULL,
    [Role_user]    INT    NOT NULL,
    PRIMARY KEY CLUSTERED ([id_user] ASC)
);

drop table Users;
CREATE SYMMETRIC KEY SymmetricKey
				WITH ALGORITHM = AES_256
				ENCRYPTION BY PASSWORD = 'AH98L@Fd2';

                INSERT INTO Users (Login_user, password_user, Role_user)
VALUES ('ahmed', '1234', 1),
       ('sect',  '0000', 0);
       select * from Users;