/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function (knex) {
    await knex.raw(`
      CREATE TABLE IF NOT EXISTS ranks (
      customer_rank VARCHAR(10) NOT NULL,
      rank_point int default 0,
      rank_discount DECIMAL(5, 2) default 0,
      point_per_dollar DECIMAL(5, 2) default 0,
      primary key(customer_rank)
      );`);
  };
  
  /**
   * @param { import("knex").Knex } knex
   * @returns { Promise<void> }
   */
  exports.down = async function (knex) {
    await knex.raw(`DROP TABLE IF EXISTS ranks;`);
  };
  