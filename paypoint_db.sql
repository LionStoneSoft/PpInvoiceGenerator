CREATE DATABASE paypoint_rs; #creates database

USE paypoint_rs; #'enters' the listed database table

CREATE TABLE customer (
	customer_id INT auto_increment PRIMARY KEY,
	first_name VARCHAR(20),
    last_name VARCHAR(30)
);

CREATE TABLE gameItems (
	item_id INT auto_increment PRIMARY KEY,
	game_name VARCHAR(30),
    item_description VARCHAR(50),
    price DECIMAL(3,2)
);

CREATE TABLE customerGameCharge (
	transaction_id INT auto_increment PRIMARY KEY,
	customer_id INT,
    item_id INT,
    transaction_date DATETIME,
    FOREIGN KEY (item_id) REFERENCES gameItems(item_id),
    FOREIGN KEY (customer_id) REFERENCES customer(customer_id)
);

DELIMITER $$

CREATE PROCEDURE PopulateCustomers(
    amount INT
)
BEGIN
    DECLARE count INT DEFAULT 1;

    WHILE count <= amount DO
		insert into customer (first_name, last_name) VALUES (CONCAT('Customer', count), 'LastName');
		set count=count+1;
    END WHILE;

END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE PopulateGameItems(
    amount INT, #amount of games
    item_description VARCHAR(30)
)
BEGIN
    DECLARE count INT DEFAULT 1;

    WHILE count <= amount DO
		insert into gameItems (game_name, item_description, price) VALUES (CONCAT('Game ', count), item_description, RAND()*(9.99-0.01)+0.01);
		set count=count+1;
    END WHILE;

END$$

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE PopulateGameCharges(
    amount INT, #amount of transactions
    customerAmount INT,
    gameItemsAmount INT
)
BEGIN
    DECLARE count INT DEFAULT 1;

    WHILE count <= amount DO
		insert into customerGameCharge (customer_id, item_id, transaction_date) VALUES ( FLOOR(RAND()*(customerAmount-1+1))+1, FLOOR(RAND()*(gameItemsAmount-1+1))+1, FROM_UNIXTIME(UNIX_TIMESTAMP('2021-05-24 00:00:00') + FLOOR(0 + (RAND() * 172800))));
		set count=count+1;
    END WHILE;

END$$

DELIMITER ;



CALL PopulateCustomers(32000);
CALL PopulateGameItems(18, 'New Skin');
CALL PopulateGameCharges(120000, 40000, 53);






