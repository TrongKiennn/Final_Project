/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    await knex.raw(`
        CREATE TABLE customers (
            fullname varchar(50) NOT NULL,
            phoneNumber varchar(11) not null primary key,
            email varchar(50),
            point int not null default 0,
            customer_rank varchar(10),
            rank_discount DECIMAL(5, 2) default 0,
            point_per_dollar DECIMAL(5, 2) default 0,
            created_at timestamp NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
            FOREIGN KEY (customer_rank) REFERENCES ranks(customer_rank)
        ) ENGINE=InnoDB;
    `);

    await knex.raw(`
       CREATE TRIGGER set_customer_rank_before_update
    BEFORE UPDATE ON customers
    FOR EACH ROW
    BEGIN
        DECLARE rank_name VARCHAR(10);
        DECLARE cus_discount DECIMAL(5, 2);
        DECLARE cus_point_per_dollar DECIMAL(5, 2);
        
        SELECT customer_rank, rank_discount, point_per_dollar
        INTO rank_name, cus_discount, cus_point_per_dollar
        FROM ranks
        WHERE NEW.point >= rank_point
        ORDER BY rank_point DESC
        LIMIT 1;

        SET NEW.customer_rank = rank_name, 
            NEW.rank_discount = cus_discount, 
            NEW.point_per_dollar = cus_point_per_dollar;
    END;

    `);

    await knex.raw(`
        CREATE TRIGGER set_customer_rank_before_insert
        BEFORE INSERT ON customers
        FOR EACH ROW
        BEGIN
            DECLARE rank_name VARCHAR(10);
            DECLARE cus_discount DECIMAL(5, 2);
            DECLARE cus_point_per_dollar DECIMAL(5, 2);
            
            
            SELECT customer_rank, rank_discount, point_per_dollar
            INTO rank_name, cus_discount, cus_point_per_dollar
            FROM ranks
            WHERE NEW.point >= rank_point
            ORDER BY rank_point DESC
            LIMIT 1;

        
            SET NEW.customer_rank = rank_name, 
                NEW.rank_discount = cus_discount, 
                NEW.point_per_dollar = cus_point_per_dollar;
        END;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TABLE IF EXISTS customers;
    `);
};
