# PruebaTecnica
Login and signin user
* Endpoit POST:
  * signin: https://localhost:44380/v1/api/signin
  * login: https://localhost:44380/v1/api/login
* Headers:
  * SecretKey: 123456
* Json example:
  * signin: {
 "name" : "Jose Lopez",
 "email" : "jose@gmail.com",
 "password" : "123456"
}
  * login: {
 "email" : "jose@gmail.com",
 "password" : "123456"
}
* Tecnologies
  * IDE Visual Studio Professional 2019
  * Asp.Net Core 5.0
  * SQL Server 2018
