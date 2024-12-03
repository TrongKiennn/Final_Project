/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('type_of_drink').del()
  await knex('type_of_drink').insert([
    {id: 1, type_name: 'Coffee'},
    {id: 2, type_name: 'MilkTea'},
    {id: 3, type_name: 'Croissant'},
    {id: 4, type_name: 'Smoothie'},
    {id: 5, type_name: 'Juice'},
    {id: 6, type_name: 'Tea'}
  ]);
};
