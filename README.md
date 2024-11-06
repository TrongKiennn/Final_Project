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

   Open your MySQL client and create a database named `pos_manager`:
   ```sql
   CREATE DATABASE pos_manager;
You need to add .env with content like this
```bash
NODE_ENV=development
MYSQL_HOST=127.0.0.1
MYSQL_PORT=3306
MYSQL_DB=pos_manager
MYSQL_USER=root
MYSQL_PASSWORD=1234
