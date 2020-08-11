# servicioinventario

o	Instrucciones para ejecutar la aplicación

Para ejecutar la aplicación basta con abrir la solución en Visual Studio y ejecutar el proyecto "ServicioInventario.WebAPI".
Está configurado para arrancar en IISExpress y abrir la página: /swagger/ui/index (interfaz gráfico de Swagger con acceso a la API).
También podemos acceder a: /hangfire (interfaz gráfico de Hangfire con acceso al trabajo periódico).

o	Breve documentación sobre el diseño, estructura de código y cualquier anotación que se quiera añadir sobre extensibilidad, mantenimiento, seguridad, rendimiento, etc, que no haya dado tiempo a implementar.

La solución se ha diseñado con los siguientes componentes:
- ServicioInventario.WebAPI: Servicio REST para ejecutar las operaciones solicitadas, también servidor para la ejecución de tareas periódicas.
- ServicioInventario.Aplicacion: Lógica de negocio donde están programadas las operaciones
- ServicioInventario.Datos: Componente para el acceso a datos con una sencilla implementación de un repositorio de información en fichero XML
ubicado en la raiz del ensamblado.
- ServicioInventario.Modelos: Componente que describe el modelo utilizado en la solición "ElementoInventario"
- ServicioInventario.AplicacionTests: Proyecto de pruebas para la lógica de negocio.

Se ha utilizado Swagger para la documentación a partir del XML generado en la construcción del proyecto.
Se ha utilizado Hangfire para la ejecución de tareas asíncronas, en concreto se ha programado un trabajo que se ejecuta
cada minuto para la notificación de los elementos caducados.*
Se ha versionado la API, siendo la actual la "1.0".

* Se pueden visualizar las operaciones recurrentes en '/hangfire/recurring', pero también observando el fichero de "output.txt" en la raiz
del ensamblado. Si hay algún elemento caducado pendiente de notificar, apacerá en el fichero.

Notas:
- No se ha incluido la implementación de seguridad debido a su complejidad y el tiempo necesario, en cualquier caso una posible implementación
sería con OAuth 2.0 y JWT a través de un endpoint para la autorización y la configuración de este servicio para el consumo de los Tokens generados.
- No se ha incluido trazabilidad, una posible solución sería con un interfaz propio que luego podríamos implementar para su uso con un desarrollo
propio o librerías existentes como Log4Net...
- Solo he añadido comentarios en los proyectos Aplicación, Modelos y WebAPI, a modo de ejemplo.
- Solo he añadido tests unitarios a la clase GestorInventario, a modo de ejemplo.
- Para las notificaciones, se podría haber desarrollado un sistema más complejo, basado en el patrón Observador, el patrón Publicador/Suscriptor, 
o mediante eventos. No obstante, por agilidad, he creado un interfaz de notificación y he realizado una implementación trivial que escribe
las notificaciones en fichero (output.txt, en la raiz del ensamblado)
- Por simplicidad y agilidad, tanto el servidor de Hangfire como el interfaz se han configurado en el mismo servicio web, pero probablemente
en producción deberían ir en aplicaciones independientes para mayor desacomplamiento, y en cualquier caso, en producción, el interfaz
gráfico iría securizado.
- No se ha desarrollado una gestión completa de los errores, por simplicidad simplemente se devuelve el error como mensaje. Sí se controlan
algunos errores como argumentos nulos o claves no encontradas.
- También por agilidad y por simplificar la solución no se ha utilizado el principio de IoC para la gestión de depencencias, en su lugar
se utiliza inyección de dependencias en constructores. En un proyecto de producción se podría utilizar MEF, Unity, etc...

o	Breve documentación sobre asunciones, razonamientos, requisitos modificados y sus motivos

- Se asume que el nombre puede tener duplicados, se incluye un id autonumérico para la identificación unívoca de los elementos.
- Al obtener un elemento con el mismo nombre, se obtiene el que tiene menor fecha de caducidad, es decir, el que caducará antes.
- Se limita el tamaño del campo nombre y tipo de ElementoInventario para que puedan ser indexado (con duplicados) en una futura base de datos.
- Se asume que todos las propiedades de ElementoInventario son obligatorias (no pueden se nulas)