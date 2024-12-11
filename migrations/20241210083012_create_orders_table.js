/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE orders (
        id INT(4) NOT NULL AUTO_INCREMENT,
        total_amount DECIMAL(10, 2) NOT NULL,
        status ENUM('completed', 'pending', 'cancelled') DEFAULT 'completed',
        user_id int(4) NOT NULL,
        created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
        updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
        PRIMARY KEY (id),
        FOREIGN KEY (user_id) REFERENCES users(id)
    ) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down =async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS orders;
    `);
};
