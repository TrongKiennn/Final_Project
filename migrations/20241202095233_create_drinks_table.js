/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE drinks (
            id int(4) NOT NULL AUTO_INCREMENT,
            drink_name varchar(50) NOT NULL,
            typeId int(4) NOT NULL,
            price decimal(10, 2) NOT NULL,
            drink_img_url VARCHAR(255) NOT NULL,
            status ENUM('available', 'unavailable') DEFAULT 'available',
            created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            PRIMARY KEY (id),
            FOREIGN KEY (typeId) REFERENCES type_of_drink(id) ON DELETE CASCADE
        ) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down =async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS drinks;
    `);
};
