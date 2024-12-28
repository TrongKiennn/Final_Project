/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
         CREATE TABLE orderItems (
            id INT(4) NOT NULL AUTO_INCREMENT,
            order_id INT(4) NOT NULL,
            drink_id INT(4) NOT NULL,
            quantity INT(4) DEFAULT 0,
            total DECIMAL(10, 2) NOT NULL,
            created_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            PRIMARY KEY (id),
            FOREIGN KEY (order_id) REFERENCES orders(id),
            FOREIGN KEY (drink_id) REFERENCES drinks(id)
            ON DELETE CASCADE ON UPDATE CASCADE  
        ) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down =async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS orderItems;
    `);
};
