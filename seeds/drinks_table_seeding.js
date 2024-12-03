/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.seed = async function (knex) {
  // Deletes ALL existing entries in the 'drinks' table
  await knex('drinks').del();

  // Inserts 30 drinks
  await knex('drinks').insert([
    { id: 1, drink_name: 'Espresso', typeId: 1, price: 2.5, drink_img_url: 'https://product.hstatic.net/1000075078/product/espressonong_612688_5fb39ee5c14840f1bf025bdf9a8a0ffd.jpg' },
    { id: 2, drink_name: 'Americano', typeId: 1, price: 3.0, drink_img_url: 'https://cdn2.fptshop.com.vn/unsafe/1920x0/filters:quality(100)/2024_3_28_638472299342509511_avatar.jpg' },
    { id: 3, drink_name: 'Latte', typeId: 1, price: 3.5, drink_img_url: 'https://file.hstatic.net/200000079049/article/ban_sao_cafe-latte-4_af4c8c67f30f471e93e13acd6b5bb67c.png' },
    { id: 4, drink_name: 'Cappuccino', typeId: 1, price: 3.8, drink_img_url: 'https://cafefabrique.com/cdn/shop/articles/Cappuccino.jpg' },
    { id: 5, drink_name: 'Mocha', typeId: 1, price: 4.0, drink_img_url: 'https://vinbarista.com/uploads/editer/images/blogs/cafe-mocha-la-gi.jpg' },
    { id: 6, drink_name: 'Milk Tea Original', typeId: 2, price: 3.0, drink_img_url: 'https://www.sugarbobotea.com/wp-content/uploads/2022/05/Original-Milk-Tea.jpg' },
    { id: 7, drink_name: 'Taro Milk Tea', typeId: 2, price: 3.5, drink_img_url: 'https://tyberrymuch.com/wp-content/uploads/2022/07/taro-milk-tea-recipe-1-720x720.jpg' },
    { id: 8, drink_name: 'Matcha Milk Tea', typeId: 2, price: 3.8, drink_img_url: 'https://greenheartlove.com/wp-content/uploads/2023/05/matcha-boba-tea-5-500x500.jpg' },
    { id: 9, drink_name: 'Oolong Milk Tea', typeId: 2, price: 3.2, drink_img_url: 'https://sugaryums.com/wp-content/uploads/2022/03/Oolong-Milk-Tea-SugarYums-4.jpg' },
    { id: 10, drink_name: 'Butter Croissant', typeId: 3, price: 2.0, drink_img_url: 'https://static01.nyt.com/images/2021/04/07/dining/06croissantsrex1/06croissantsrex1-square640.jpg' },
    { id: 11, drink_name: 'Chocolate Croissant', typeId: 3, price: 2.5, drink_img_url: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNG6KtGEf4WC8B-GE5YtResm81uftg_tBMBg&s' },
    { id: 12, drink_name: 'Almond Croissant', typeId: 3, price: 2.8, drink_img_url: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTDqosukQPunjea8nM_60G9rJ1FcLNhij1rkg&s' },
    { id: 13, drink_name: 'Strawberry Smoothie', typeId: 4, price: 4.0, drink_img_url: 'https://www.acouplecooks.com/wp-content/uploads/2020/04/Strawberry-Smoothie-003.jpg' },
    { id: 14, drink_name: 'Mango Smoothie', typeId: 4, price: 4.2, drink_img_url: 'https://cdn.loveandlemons.com/wp-content/uploads/2023/05/mango-smoothie.jpg' },
    { id: 15, drink_name: 'Avocado Smoothie', typeId: 4, price: 4.5, drink_img_url: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQxSKjWihte0ADtXAlqOF5s8p20MbbZULUOyA&s' },
    { id: 16, drink_name: 'Banana Smoothie', typeId: 4, price: 4.0, drink_img_url: 'https://yoursmoothieguide.com/wp-content/uploads/2022/01/Banana-Milkshake-7-1.jpg' },
    { id: 17, drink_name: 'Orange Juice', typeId: 5, price: 3.5, drink_img_url: 'https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/Orangejuice.jpg/1200px-Orangejuice.jpg' },
    { id: 18, drink_name: 'Apple Juice', typeId: 5, price: 3.7, drink_img_url: 'https://www.sharmispassions.com/wp-content/uploads/2017/03/AppleJuice3.jpg' },
    { id: 19, drink_name: 'Grape Juice', typeId: 5, price: 3.9, drink_img_url: 'https://www.anediblemosaic.com/wp-content/uploads//2023/08/homemade-grape-juice-with-concord-grapes-featured-image.jpg' },
    { id: 20, drink_name: 'Carrot Juice', typeId: 5, price: 3.5, drink_img_url: 'https://healthytasteoflife.com/wp-content/uploads/2021/09/carrot-juice-recipe-blender-or-juicer-simple.jpg' },
    { id: 21, drink_name: 'Green Tea', typeId: 6, price: 2.5, drink_img_url: 'https://images.squarespace-cdn.com/content/v1/54e730d4e4b0bc2e9a8f31d0/1528278433321-0S0FTSBERLAMKUEMEZNW/glass+mugs+of+green+tea.jpg' },
    { id: 22, drink_name: 'Chamomile Tea', typeId: 6, price: 2.8, drink_img_url: 'https://cdn.mos.cms.futurecdn.net/wJ9DfTGkTWqa2z9si3cysS-1200-80.jpg' },
    { id: 23, drink_name: 'Black Tea', typeId: 6, price: 2.3, drink_img_url: 'https://www.sharmispassions.com/wp-content/uploads/2021/05/black-tea-recipe2.jpg' },
    { id: 24, drink_name: 'Lemon Tea', typeId: 6, price: 2.7, drink_img_url: 'https://www.sharmispassions.com/wp-content/uploads/2011/04/LemonTea1.jpg' },
    { id: 25, drink_name: 'Mint Tea', typeId: 6, price: 2.9, drink_img_url: 'https://minimalistbaker.com/wp-content/uploads/2023/02/Mint-Tea-SQUARE.jpg' },
    { id: 26, drink_name: 'Honey Lemonade', typeId: 5, price: 3.6, drink_img_url: 'https://thymeandjoy.com/wp-content/uploads/2022/02/honey-lemonade-56224148-1-e1645795886453.jpg' },
    { id: 27, drink_name: 'Coconut Smoothie', typeId: 4, price: 4.5, drink_img_url: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRoL0O0ydGoZtfRDMkKIICuGup56qFcels6Gw&s' },
    { id: 28, drink_name: 'Mocha Frappe', typeId: 1, price: 4.8, drink_img_url: 'https://deliciouslyorganic.net/wp-content/uploads/2011/01/Mocha-Frappe-2-Small.jpg' },
    { id: 29, drink_name: 'Thai Tea', typeId: 2, price: 3.4, drink_img_url: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTAAubXL4w8qwTDURx5mqGVtc2iLxr68ADPzw&s' },
    { id: 30, drink_name: 'Peach Iced Tea', typeId: 6, price: 3.2, drink_img_url: 'https://delight-fuel.com/wp-content/uploads/2020/07/Peach_Ice_Tea_3-735x1103.jpg' },
    { id: 31, drink_name: 'Passion Fruit Tea', typeId: 6, price: 3.4, drink_img_url: 'https://images.ctfassets.net/092wi7ilt2cf/4ZodurBqH0Tw9xJMsSD1LN/4416305d57409fce8603256d9a27a912/Passion-Fruit-Iced-Tea-Hero-11.jpg' },
    { id: 32, drink_name: 'Coconut Coffee', typeId: 1, price: 4.2, drink_img_url: 'https://nguyencoffeesupply.com/cdn/shop/articles/Coconut_Iced_Coffee_2.jpg?v=1605816185' },
    { id: 33, drink_name: 'Hazelnut Latte', typeId: 1, price: 3.9, drink_img_url: 'https://www.texanerin.com/content/uploads/2023/03/hazelnut-latte-1200-image.jpg' },
    { id: 34, drink_name: 'Blueberry Smoothie', typeId: 4, price: 4.7, drink_img_url: 'https://www.wellplated.com/wp-content/uploads/2023/12/Best-Blueberry-Smoothie-Recipe.jpg' },
    { id: 35, drink_name: 'Matcha Latte', typeId: 1, price: 4.0, drink_img_url: 'https://file.hstatic.net/200000079049/file/matcha-latte-da_ab943a159ce14b1e8ae036013eaee99d.jpeg' },
    { id: 36, drink_name: 'Raspberry Lemonade', typeId: 5, price: 3.5, drink_img_url: 'https://www.acouplecooks.com/wp-content/uploads/2021/05/Raspberry-Lemonade-007s.jpg' },
    { id: 37, drink_name: 'Peach Smoothie', typeId: 4, price: 4.3, drink_img_url: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQogYnSMm2WEs9NBviFcIOvIBeGyUFrJQffKQ&s' },
    { id: 38, drink_name: 'Iced Mocha', typeId: 1, price: 4.1, drink_img_url: 'https://vibrantlygfree.com/wp-content/uploads/2023/07/iced-mocha-1.jpg' },
    { id: 39, drink_name: 'Lychee Tea', typeId: 6, price: 3.6, drink_img_url: 'https://ganeshaeksanskriti.com/cdn/shop/files/GANESHA-24NOV20229LYCHEEICETEA14855_f86b7f15-007e-4c56-941c-6a46af767dbf.jpg?v=1703911443&width=1946' },
    { id: 40, drink_name: 'Mentho Mint Boba Milk Tea', typeId: 2, price: 3.9, drink_img_url: 'https://merokinmel.com/storage/products/thumbnail/652951953c7b2.png' },
  ]);
}
