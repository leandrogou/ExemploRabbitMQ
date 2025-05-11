# ExemploRabbitMQ
Exemplo de criação e consumo filas RabbitMQ

Antes de rodar, é preciso subir uma imagem docker. No cmd digitar o comando:
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
