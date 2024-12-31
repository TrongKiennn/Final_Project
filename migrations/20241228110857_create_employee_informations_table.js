/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE employeeInformations (
            id int(4) NOT NULL AUTO_INCREMENT,
            user_id int(4) NOT NULL, 
            fullname varchar(50) NOT NULL,
            dateOfBirth date not null,
            phoneNumber varchar(11) not null,
            gender enum('male', 'female', 'other') NOT NULL, 
            position ENUM('employee','manager') default null,
            hire_date timestamp NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            PRIMARY KEY (id),
            FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
        ) ENGINE=InnoDB;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS employeeInformations;
    `);
};
