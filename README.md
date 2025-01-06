# Coffee Shop POS System - WinUI 3 Project

This project is a comprehensive Point of Sale (POS) system designed for coffee shops, built using WinUI 3. Our POS system supports seamless operations by managing orders, inventory, and customer relationships in one platform. Key features include:

- **User Management:** Secure login and logout functionality to control access.
- **Event Scheduling:** Automated event reminders for customers and staff to ensure preparation and timely attendance.
- **Order Customization:** Options for drink modifications, allowing personalized orders for customers.
- **Recipe Suggestions:** Quick access to drink recipes, ideal for helping new staff remember complex orders.
- **Revenue Tracking:** Daily revenue reports for simplified business analysis.
- **Table Management:** Organized order tracking by table, supporting adding or removing items as needed.
- **Employee Management:** Role-based employee management with time-tracking and attendance logging.
- **Inventory Management:** Real-time inventory monitoring with alerts for low-stock items.
- **Customer Loyalty Program:** Tracks customer purchase history and offers rewards for frequent patrons.

Our system enhances the efficiency and customer experience in a coffee shop environment, providing a valuable tool for both staff and management.

## Project Milestones

This project is structured into three milestones. Currently, we are in **weeks 4–7**, working towards releasing the first software version as **Milestone 01**.

### Milestone 01: Initial Release

For Milestone 01, we have implemented several core features to ensure the system's functionality and usability, including:

- **User Management**
- **Event Scheduling**
- **Create migration for database with docker and node**

These features provide a foundation for the system, meeting essential operational needs and setting the stage for additional functionality in future milestones.

## Installation Instructions

Follow these steps to install and set up the Coffee Shop POS System.

### Prerequisites

Ensure the following are installed on your system:

- **Visual Studio 2022** (with .NET Desktop Development workload)
- **.NET 6 SDK** or higher
- **WinUI 3** (compatible with the .NET SDK)

### Installation Steps

1. **Clone the Repository**

   Clone this repository to your local machine using Git:
   ```bash
   git clone git@github.com:TrongKiennn/Final_Project.git
   cd POS_App

## Database Setup and Connection

This section will guide you through setting up and connecting to the MySQL database for the Coffee Shop POS System.

### Prerequisites

Ensure you have:
- **MySQL** installed and running on your local machine or server.
- A **MySQL client** or **command-line tool** to interact with the database.

### Step-by-Step Guide

1. **Create the Database**
Follow [this link](https://tdquang7.notion.site/H-ng-d-n-t-o-nhanh-backend-server-v-i-database-Postgres-c5f6e163a27e4882ac22fe32774b1a3c?pvs=4) for instructions on setting up a backend server with a database connection.

   Alternatively, if using MySQL, open your MySQL client and create a database named `pos_manager`:
   ```sql
   CREATE DATABASE pos_manager;
   
2. Set Up Environment Variables
   Create a .env file in the root directory of your project with the following content:
   ```bash
   NODE_ENV=development
   MYSQL_HOST=127.0.0.1
   MYSQL_PORT=3306
   MYSQL_DB=pos_manager
   MYSQL_USER=root
   MYSQL_PASSWORD=1234

3. Install Database Dependencies
Run the following command to install necessary MySQL dependencies (e.g., mysql2 for Node.js):

   ```bash
   npm install mysql2
4. Open the Project in Visual Studio
Launch Visual Studio and open the cloned project folder.
6. Build and Run the Project
- **Build the project to ensure all dependencies are correctly set up by going to Build > Build Solution.**
- **Run the application using the "Start" button in Visual Studio or press F5. This will launch the POS system in debug mode.**

## Milestone Evaluation Criteria

To ensure consistent progress and quality, each milestone will be assessed based on the following criteria:

### Milestone 01: Initial Release (Weeks 4–7)

- **UI/UX (20%)**  
   - The interface should be clean and logically organized.
   - On login, if the user enters an incorrect username or password, display:  
     `"UserName or Password is incorrect!"`
   - On registration, if the username already exists, display:  
     `"Account already exists"`

- **Design Patterns / Architecture (20%)**  
   - The project follows the **MVVM architecture** to ensure a clear separation of concerns.
   - Each function and class includes comments detailing its functionality for improved code readability.

- **Teamwork - Git Flow (10%)**  
   - The team uses **GitHub** and **Trello** to organize and track development:
     - Trello: [Project Board](https://trello.com/b/N54ELBhQ/l%E1%BA%ADp-trinh-win)
     - GitHub: [Repository](https://github.com/TrongKiennn/Final_Project)

- **Quality Assurance (20%)**  
   - Quality assurance standards will be followed in upcoming milestones to ensure code quality, reliability, and maintainability.
 
### Milestone 02: Intermediate Development (Weeks 8–11)
The team has completed 7 hours of work for **Milestone 2**, focusing on two main features:

1. **OrderPage (4 hours):** Developed the interface for placing orders, customizing products, displaying order details, and supporting payment via pop-ups.  
2. **Search and Sort (3 hours):** Implemented functionality to search and sort products by name.  [ProjectProposal](https://docs.google.com/document/d/1kSat9CqWG8-aWRFB-62Lu82mtGdHkVtC2Y2KuRkuEpE/edit?usp=sharing)

Both features have been thoroughly tested and are operating as expected, meeting the milestone timeline.

Task Assignment Table:
| 22120151                                                                                                                             | 22120167                                                                                                                             |
|----------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------|
| FrontEnd for OrderPage                                                                                                                | BackEnd for OrderPage                                                                                                                 |
| navigationView                                                                                                                        | filter                                                                                                                                |
| Items view                                                                                                                            | search                                                                                                                                |
| searchView                                                                                                                            | display items                                                                                                                         |
| filterView                                                                                                                            | save items into database                                                                                                              |
| popup for continue to payment                                                                                                         | handle popup for custom an item when order                                                                                            |
| popup for custom an item when order                                                                                                   | handle popUp for continue payment                                                                                                     |
| Content for Record                                                                                                                    | Video Record                                                                                                                          |
| Unit test                                                                                                                             | Unit test                                                                                                                             |
- **UI/UX (20%)**  
   - The interface should be cleaned and logically organized.
   - When click on an items, If user does not choose any custom items, the default will be seted for this items.
   - UI: [UI link check](https://docs.google.com/document/d/1XnmnEVnmcCzOUzSOjxavhJ2Yj78g9KM-R-ONCt2x0Jw/edit?usp=sharing)

- **Design Patterns / Architecture (20%)**  
   - The project follows the **MVVM architecture** to ensure a clear separation of concerns.
   - Each function and class includes comments detailing its functionality for improved code readability.
 
- **Advantage Topic**
   - Use NavigationView to navigate between pages.
   - Export bills to PDF files.

- **Teamwork - Git Flow (10%)**  
   - The team uses **GitHub** and **Trello** to organize and track development:
     - Trello: [Project Board](https://trello.com/b/N54ELBhQ/l%E1%BA%ADp-trinh-win)
     - GitHub: [Repository](https://github.com/TrongKiennn/Final_Project)

- **Quality Assurance (20%)**  
   - Unit test some functions: Search, SortByName, Loadata.
 
Link Video Demo Milestone 2: 

### Milestone 03: Final Release (Weeks 12–15)
The team has completed 7 hours of work for **Milestone 2**, focusing on two main features:

1. **Suggestions for drink recipes (1 hours):** After completing the order, you can click on the suggestions to display the drink recipes in the order, making it convenient for staff, especially new employees, who might struggle to remember too many recipes.
2. Employee Management (1 hour)
- Manage employees with roles such as:
  - **Manager**
  - **Waiter**
  - **Staff**
- Calculate working hours for each employee.
- Track attendance efficiently.
3. Ingredient Management (1 hours)
- Track quantities of ingredients such as:
  - **Milk**
  - **Coffee**
  - **Sugar**
  - **Ice**
- Provide alerts when ingredients are low or running out.
5. Loyalty Customer Management (2 hours)
- Store purchase history for customers who are members of the shop.
- Accumulate points for customers to redeem vouchers or gifts.
6. Real-time Inventory Update (1 hour - Advanced Feature)
- Automatically update ingredient quantities in the system with each new order.
- Helps manage inventory more efficiently and reduces waste.
7. VIP Customer Upgrade (1 hour - Advanced Feature)
- Customers who make more than 50 purchases will be upgraded to VIP status.
- VIP customers will receive discounts on future purchases.
8. **Revenue Statistics (2 hours):** Display the revenue statistics for the shop by day, week, and month.  [ProjectProposal](https://docs.google.com/document/d/1kSat9CqWG8-aWRFB-62Lu82mtGdHkVtC2Y2KuRkuEpE/edit?usp=sharing)

Both features have been thoroughly tested and are operating as expected, meeting the milestone timeline.

Task Assignment Table:
| 22120151                                                                                                                             | 22120167                                                                                                                             |
|----------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------|
| FrontEnd for OrderPage                                                                                                                | BackEnd for OrderPage                                                                                                                 |
| navigationView                                                                                                                        | filter                                                                                                                                |
| Items view                                                                                                                            | search                                                                                                                                |
| searchView                                                                                                                            | display items                                                                                                                         |
| filterView                                                                                                                            | save items into database                                                                                                              |
| popup for continue to payment                                                                                                         | handle popup for custom an item when order                                                                                            |
| popup for custom an item when order                                                                                                   | handle popUp for continue payment                                                                                                     |
| Content for Record                                                                                                                    | Video Record                                                                                                                          |
| Unit test                                                                                                                             | Unit test                                                                                                                             |
- **UI/UX (20%)**  
   - The interface should be cleaned and logically organized.
   - When click on an items, If user does not choose any custom items, the default will be seted for this items.
   - UI: [UI link check](https://docs.google.com/document/d/1XnmnEVnmcCzOUzSOjxavhJ2Yj78g9KM-R-ONCt2x0Jw/edit?usp=sharing)

- **Design Patterns / Architecture (20%)**  
   - The project follows the **MVVM architecture** to ensure a clear separation of concerns.
   - Each function and class includes comments detailing its functionality for improved code readability.
 
- **Advantage Topic**
   - Use NavigationView to navigate between pages.
   - Export bills to PDF files.

- **Teamwork - Git Flow (10%)**  
   - The team uses **GitHub** and **Trello** to organize and track development:
     - Trello: [Project Board](https://trello.com/b/N54ELBhQ/l%E1%BA%ADp-trinh-win)
     - GitHub: [Repository](https://github.com/TrongKiennn/Final_Project)

- **Quality Assurance (20%)**  
   - Unit test some functions: Search, SortByName, Loadata.
 
Link Video Demo Milestone 2: 

By setting clear and actionable criteria, we aim to ensure each milestone meets its objectives while maintaining high standards for usability, reliability, and scalability.
