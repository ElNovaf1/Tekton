# Tekton Test Challenge by Joel Aguilar
Hola. En este repositorio encontrarán el código fuente de la solución desarrollada por mi persona al reto enviado en esta semana. Me pareció un buen ejercicio para calentar los dedos y demostrar algunas de mis habilidades como backend developer.

Espero no tengan ningún inconveniente haciendo correr el proyecto en sus equipos. Buen día! :D.
## Table of Contents
- [Resumen](#Resumen)
- [Correr el Proyecto](#Correrlo)
- [Estructura](#Estructura)
- [Patrones](#Patrones)
- [Comentarios](#Comentarios)

## Resumen
El proyecto consiste en una solución desarrollada utilizando C#(.net7). El ejercicio se realizó utilizando "Clean architecture" para su diseño por lo que dentro de este encontraran proyectos que representan las capas de Dominio, Infraestructura y Servicios que son la base de este tipo de diseño. El proyecto se conecta a una base de datos SQL Server 2022.

Adicionalmente se han implementado diversos patrones de diseños para cumplir con las exigencias del reto y a la vez facilitar un poco el desarrollo del mismo. 

### Correrlo
Para poder correr este proyecto en su sistema favor seguir los siguientes pasos:

1- Restaurar el respaldo de la base de datos "tekton.bak" en una instancia de SQL Server 2022 ya sea local o disponible en la red. Este archivo de respaldo puedes encontrarlo en la raiz del repositorio (justo al lado de este README :D).

2 - Descargar la solución a tu equipo ya sea desde el navegador o conectando tu IDE a traves de Git.

3 - Abrir la solución y editar los archivos JSON de configuración "appdettings.json" de los proyectos "Tekton.API", "Tekton.UnitTests" y "Tekton.IntegrationTest". Las llaves que deberas actualizar son:
 - sqlConnection : Actualizarla con la cadena de conexión que apunte al backup restaurado en el paso 1.
 - IntegrationTestBaseUri: Actualizar el puerto de la URL con el configurado en tu instancia de desarrollo (la url y puertos del localhost cuando corres tus RestAPIs en tu instancia de desarrollo). Por ejemplo en mi caso utilizo el puerto 7238 por lo que mi llave IntegrationTestBaseUri queda con el valor "https://localhost:7238/Products/" si en tu caso utilizas por ejemplo el puerto 7231 tu llave deberá quedar con el valor "https://localhost:7231/Products/".

4- El proyecto realiza request a MockApi por lo que deberas asegurarte que tu entorno de desarrollo tenga acceso a la url "https://65b95de8b71048505a8abfb6.mockapi.io/api/tekton/Discounts/" que se encuentra en la llave "DiscountAPIUrl" de los archivos de configuración.

Listo!. Con eso ya deberías poder correr la solución y probarla. Si tienes algún inconveniente no dudes en escribirme al correo novaf1@outlook.com para apoyarte.

## Estructura
Dentro de la solución podrás encontrar la siguiente estructura de proyectos:

-**01 Tekton.Domain** : Esta es la capa más interna. Alberga entre otras cosas entidades y otros objetos de valor.\
-**02 Tekton.Infrastructure** : Acá se encuentran las interfaces e implementaciones de los repositorios que conectan con la base de datos o sistemas externos. Por ejemplo acá se encuentran nuestro repositorio generico para conectarnos a nuestra DB, nuestro repositorio HTTP para hacer request a APIs externas, nuestras unidades de trabajo y decoradores utilizados para acceder a nuestra cache.\
-**03 Tekton.Service** : Acá se encuentran las interfaces e implementaciones de nuestra lógica de negocio y los DTOs utilizados para enviar y recibir datos de las capas que requieran estos servicios.\
-**Tekton.API** : En este proyecto se encuentran los controladores que consumen los servicios de la capa Tekton.Service. \
-**Tekton.UnitTests** : Acá podremos encontrar los casos y pruebas unitarias utilizados para iniciar el desarrollo del proyecto utilizando TDD. Se decidió realizar el desarrollo utilizando pruebas unitarias el TDD de las capas mas externas de proyecto.\
-**Tekton.IntegrationTest** : Acá podremos encontrar los casos y pruebas de integración utilizados para continuar el desarrollo del proyecto utilizando TDD. Se decidió realizar el desarrollo utilizando pruebas de integración para el TDD de las capas mas internas del proyecto ya que se utilizan llamados a Entity Framework(código externo).\

## Patrones
A continuación detallo listado de patrones de desarrollo utilizados en el proyecto:

-**Patrón Repositorio** : Utilizado para abstraer nuestra lógica de acceso a datos.\
-**Patrón Repositorio Genérico** : Utilizado para desacoplar el tipo de entidad de nuestra operaciones a la base de datos de forma que utilizando un solo repositorio podamos trabajar con cualquier tipo de entidad.\
-**Patrón Unit of Work** : Utilizado para abstraer la lógica de agrupación de "Transacciones" de forma que se pueda realizar un solo commit de las mismas a nuestro repositorio de datos o bien realizar Rollbacks.\
-**Patrón Decorator** : Utilizamos un Decorator para extender la funcionalidad de nuestro repositorio genérico de forma que si lo deseamos podamos guardar y recuperar registros de nuestra base de datos en cache.\
## Comentarios
Me tomé la libertad de adaptar algunos de los requerimientos del ejercicio, específicamente el requerimiento de guardar un diccionario de datos en cache para la entidad "Status": Ya que decidí implementar un repositorio genérico me pareció mas adecuado extender la funcionalidad del mismo mediante decorator para guardar y recuperar de cache los registros de cualquier entidad que utilice el repositorio genérico.\

Adicional a lo solicitado agregue un par de metodos adicionales (GetAll y Delete).

Puedes encontrar los logs de request dentro de la carpeta logs en el proyecto Tekton.API\

Gracias por la oportunidad de participar de este proceso.
