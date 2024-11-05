/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE users (
            id int NOT NULL AUTO_INCREMENT,
            email varchar(50) NOT NULL,
            fb_id varchar(50) DEFAULT NULL,
            gg_id varchar(50) DEFAULT NULL,
            password varchar(50) NOT NULL,
            salt varchar(50) DEFAULT NULL,
            last_name varchar(50) NOT NULL,
            first_name varchar(50) NOT NULL,
            phone varchar(20) DEFAULT NULL,
            role enum('user', 'admin', 'shipper') NOT NULL DEFAULT 'user',
            avatar json DEFAULT NULL,
            status int NOT NULL DEFAULT '1',
            created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            PRIMARY KEY (id),
            UNIQUE KEY email (email)
        ) ENGINE=InnoDB;
    `);

    await knex.raw(`
        CREATE TABLE events (
            id int(11) NOT NULL AUTO_INCREMENT,
            user_id int(11) DEFAULT NULL,
            event_name varchar(50) NOT NULL,
            date datetime NOT NULL,
            customer_name varchar(50) NOT NULL,
            status int(11) NOT NULL DEFAULT '0',
            created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            note varchar(50) DEFAULT 'Không có ghi chú!' COMMENT 'empty',
            email varchar(50) NOT NULL DEFAULT 'null' COMMENT 'empty',
            phone_number varchar(50) NOT NULL DEFAULT 'null' COMMENT 'EMPTY',
            table_number int(11) DEFAULT '0',
            PRIMARY KEY (id)
        ) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4;
    `);
};

exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS users;
    `);

    await knex.raw(`
        DROP TABLE IF EXISTS events;
    `);
};
