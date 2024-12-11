/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE users (
            id int(4) NOT NULL AUTO_INCREMENT,
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
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS users;
    `);
};
