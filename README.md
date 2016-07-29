# _Arms Generator_

#### _C# and CSS Team Week Project at Epicodus, 07.01.2016_

#### By _**Callan McNulty, Alan Denison, Niem Nguyen, and Amanda Daly**_

## Description

_This is a blazon parser built using C#, SQL, Razor, JavaScript, JQuery, HTML and CSS.

## Setup/Installation Requirements



1. From Powershell, clone this repository using the command 'git clone (http://www.mono-project.com/docs/getting-started/install/windows/)
2. Navigate to project directory
3. Run the command 'dnu restore' to get dependencies
4. Use the command 'dnx kestrel' to run the kestrel server
5. Open Google Chrome and go to: localhost:5004

DATABASE INSTRUCTIONS  
from sqlcmd, enter the following lines:
CREATE DATABASE blazon_database;  
GO  
USE DATABASE blazon_database;  
GO  
CREATE TABLE blazons(id INT IDENTITY (1,1), name VARCHAR(255), blazon VARCHAR(400), shape(int));  
GO


## Known Bugs

_None are known at this time.  

## Support and contact details

_send questions to alan79td@gmail.com

## Technologies Used

_C#, SQL, Razor, JavaScript, JQuery, CSS_

### License

*The MIT License (MIT)

Copyright (c) 2016 Callan McNulty, Alan Denison, Niem Nguyen, and Amanda Daly

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*

Copyright (c) 2016 **_Amanda Daly_**
