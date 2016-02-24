#BlindsManagementSystem

Live demo :http://http://blindsmanagementsystem.azurewebsites.net

####ASP.NET MVC project for small blinds manufacturer. 
The project is designed to act as public website, advertising the products and provide option for selling them. It also manages the manufacture process to a certain level - calculating exact prices based on materials used, updating the amounts in stock and control the supplies available. The system automatically calculates material expense used and manufacture days needed for any order made.
The system provides following options:
##Users
Users can browse all products the company offers and see all the public content provided, such as information, gallery and technical instruction.
Users have access to “About us”, “Contacts”, “Our partners” section.
Registered users can make order, providing their own blind sizes and choosing from represented types and materials.
Registered users can see all owned orders and view detailed information for each of them.
##Administrators
Administrators have rights to access the “Administration area” which provides capabilities: 
Create, update, delete types of blinds.
Create, update, delete pictures to the gallery for each type.
Create, update, delete rails for blind type.
Create, update, delete fabrics and lamels for blind type.
Create, update, delete components for blind type.
Create, update, delete already created blinds.
##Project architecture
1. Web – based on MVC architecture
	* App_Start – configuration classes
	* Areas – Public and Administration
	* Content – CSS files used in project
	* Controllers – account and home controllers
	* Infrastructure – Mapping, Caching, IoC
	* Libraries – outer libraries used
	* Models – model classes for user functionality
	* Scripts – scripts used in the project
	* Views – Razor views
2. Data
	* Contracts – Holding Interfaces
	* Data – Holding the Repositories and Context classes and interfaces
	* Models – Holding classes for the database models
3. Common
	* ExtensionMetods – methods helping the project workflow
	* GlobalConstants – string constants in Bulgarian



	