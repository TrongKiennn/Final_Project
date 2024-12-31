/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('ranks').del()
  await knex('ranks').insert([
    {customer_rank: 'Bronze', rank_point: 10 },
    {customer_rank: 'Silver', rank_point: 20},
    {customer_rank: 'Gold', rank_point: 50}
  ]);
};
