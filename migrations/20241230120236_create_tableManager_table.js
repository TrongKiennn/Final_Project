/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE tableManager (
        id INT(4) NOT NULL AUTO_INCREMENT,
        status ENUM('occupied', 'available', 'reserved') DEFAULT 'available',
        order_id int(4) ,
        PRIMARY KEY (id),
        FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE
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
