/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
    // Tạo bảng ingredients
    await knex.raw(`
        CREATE TABLE ingredients (
            ingredient_id INT AUTO_INCREMENT PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            unit VARCHAR(50) NOT NULL,
            status ENUM('available', 'low', 'out_soon') DEFAULT 'available',
            stock INT DEFAULT 0
        ) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4;
    `);

    await knex.raw(`
        CREATE TRIGGER update_status_on_stock_change_update
        BEFORE UPDATE ON ingredients
        FOR EACH ROW
        BEGIN
            IF NEW.unit = 'gram' THEN
                IF NEW.stock < 20 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 100 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            ELSEIF NEW.unit = 'mililit' THEN
                IF NEW.stock < 500 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 1000 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            ELSEIF NEW.unit IN ('quả', 'củ') THEN
                IF NEW.stock < 5 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 10 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            ELSEIF NEW.unit = 'lá' THEN
                IF NEW.stock < 20 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 50 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            END IF;
        END;
    `);

    await knex.raw(`
        CREATE TRIGGER update_status_on_stock_change_insert
        BEFORE INSERT ON ingredients
        FOR EACH ROW
        BEGIN
            IF NEW.unit = 'gram' THEN
                IF NEW.stock < 20 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 100 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            ELSEIF NEW.unit = 'mililit' THEN
                IF NEW.stock < 500 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 1000 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            ELSEIF NEW.unit IN ('quả', 'củ') THEN
                IF NEW.stock < 5 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 10 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            ELSEIF NEW.unit = 'lá' THEN
                IF NEW.stock < 20 THEN
                    SET NEW.status = 'out_soon';
                ELSEIF NEW.stock <= 50 THEN
                    SET NEW.status = 'low';
                ELSE
                    SET NEW.status = 'available';
                END IF;
            END IF;
        END;
    `);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
    await knex.raw(`
        DROP TRIGGER IF EXISTS update_status_on_stock_change_insert;
    `);

    await knex.raw(`
        DROP TRIGGER IF EXISTS update_status_on_stock_change_update;
    `);

    await knex.raw(`
        DROP TABLE IF EXISTS ingredients;
    `);
};
