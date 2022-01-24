FROM nginx

# NOTE!!!
# Even though we keep nginx.local.conf inside /nginx solution folder,
# docker tool is not aware of it since it is only a Visual Studio construct.
COPY nginx/nginx.local.conf /etc/nginx/nginx.conf
#COPY nginx/api-local.sparkrosedigital_template.crt /etc/ssl/certs/api-local.sparkrosedigital_template.com.crt
#COPY nginx/api-local.sparkrosedigital_template.key /etc/ssl/private/api-local.sparkrosedigital_template.com.key