#!/bin/bash

# Establecer la contraseña en la variable de entorno PGPASSWORD
export PGPASSWORD=admin

# Ejecutar el comando psql
psql -U admin -d entratix_db -h localhost -p 5434 -f db.sql

# Limpiar la variable de entorno PGPASSWORD
unset PGPASSWORD

# Esperar 3 segundos
sleep 3
