# servicioinventario

o	Instrucciones para ejecutar la aplicaci�n

Para ejecutar la aplicaci�n basta con abrir la soluci�n en Visual Studio y ejecutar el proyecto "ServicioInventario.WebAPI".
Est� configurado para arrancar en IISExpress y abrir la p�gina: /swagger/ui/index (interfaz gr�fico de Swagger con acceso a la API).
Tambi�n podemos acceder a: /hangfire (interfaz gr�fico de Hangfire con acceso al trabajo peri�dico).

o	Breve documentaci�n sobre el dise�o, estructura de c�digo y cualquier anotaci�n que se quiera a�adir sobre extensibilidad, mantenimiento, seguridad, rendimiento, etc, que no haya dado tiempo a implementar.

La soluci�n se ha dise�ado con los siguientes componentes:
- ServicioInventario.WebAPI: Servicio REST para ejecutar las operaciones solicitadas, tambi�n servidor para la ejecuci�n de tareas peri�dicas.
- ServicioInventario.Aplicacion: L�gica de negocio donde est�n programadas las operaciones
- ServicioInventario.Datos: Componente para el acceso a datos con una sencilla implementaci�n de un repositorio de informaci�n en fichero XML
ubicado en la raiz del ensamblado.
- ServicioInventario.Modelos: Componente que describe el modelo utilizado en la solici�n "ElementoInventario"
- ServicioInventario.AplicacionTests: Proyecto de pruebas para la l�gica de negocio.

Se ha utilizado Swagger para la documentaci�n a partir del XML generado en la construcci�n del proyecto.
Se ha utilizado Hangfire para la ejecuci�n de tareas as�ncronas, en concreto se ha programado un trabajo que se ejecuta
cada minuto para la notificaci�n de los elementos caducados.*
Se ha versionado la API, siendo la actual la "1.0".

* Se pueden visualizar las operaciones recurrentes en '/hangfire/recurring', pero tambi�n observando el fichero de "output.txt" en la raiz
del ensamblado. Si hay alg�n elemento caducado pendiente de notificar, apacer� en el fichero.

Notas:
- No se ha incluido la implementaci�n de seguridad debido a su complejidad y el tiempo necesario, en cualquier caso una posible implementaci�n
ser�a con OAuth 2.0 y JWT a trav�s de un endpoint para la autorizaci�n y la configuraci�n de este servicio para el consumo de los Tokens generados.
- No se ha incluido trazabilidad, una posible soluci�n ser�a con un interfaz propio que luego podr�amos implementar para su uso con un desarrollo
propio o librer�as existentes como Log4Net...
- Solo he a�adido comentarios en los proyectos Aplicaci�n, Modelos y WebAPI, a modo de ejemplo.
- Solo he a�adido tests unitarios a la clase GestorInventario, a modo de ejemplo.
- Para las notificaciones, se podr�a haber desarrollado un sistema m�s complejo, basado en el patr�n Observador, el patr�n Publicador/Suscriptor, 
o mediante eventos. No obstante, por agilidad, he creado un interfaz de notificaci�n y he realizado una implementaci�n trivial que escribe
las notificaciones en fichero (output.txt, en la raiz del ensamblado)
- Por simplicidad y agilidad, tanto el servidor de Hangfire como el interfaz se han configurado en el mismo servicio web, pero probablemente
en producci�n deber�an ir en aplicaciones independientes para mayor desacomplamiento, y en cualquier caso, en producci�n, el interfaz
gr�fico ir�a securizado.
- No se ha desarrollado una gesti�n completa de los errores, por simplicidad simplemente se devuelve el error como mensaje. S� se controlan
algunos errores como argumentos nulos o claves no encontradas.
- Tambi�n por agilidad y por simplificar la soluci�n no se ha utilizado el principio de IoC para la gesti�n de depencencias, en su lugar
se utiliza inyecci�n de dependencias en constructores. En un proyecto de producci�n se podr�a utilizar MEF, Unity, etc...

o	Breve documentaci�n sobre asunciones, razonamientos, requisitos modificados y sus motivos

- Se asume que el nombre puede tener duplicados, se incluye un id autonum�rico para la identificaci�n un�voca de los elementos.
- Al obtener un elemento con el mismo nombre, se obtiene el que tiene menor fecha de caducidad, es decir, el que caducar� antes.
- Se limita el tama�o del campo nombre y tipo de ElementoInventario para que puedan ser indexado (con duplicados) en una futura base de datos.
- Se asume que todos las propiedades de ElementoInventario son obligatorias (no pueden se nulas)