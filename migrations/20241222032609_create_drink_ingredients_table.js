/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE drink_ingredients (
            drink_id INT,                                
            ingredient_id INT,                          
            quantity DOUBLE NOT NULL,                    
            PRIMARY KEY (drink_id, ingredient_id),       
            FOREIGN KEY (drink_id) REFERENCES drinks(id) 
            ON DELETE CASCADE ON UPDATE CASCADE,     
            FOREIGN KEY (ingredient_id) REFERENCES ingredients(ingredient_id) 
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
        DROP TABLE IF EXISTS drink_ingredients;
    `);
};
