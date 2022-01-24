FROM postgres

COPY sql/InitializeSparkRoseDigital_TemplateDb.sql /docker-entrypoint-initdb.d/
RUN echo "en_US UTF-8" >> /etc/locale.gen && locale-gen
