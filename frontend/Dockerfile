# Estágio de Build
FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Estágio do Servidor
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Atenção: ajuste o caminho 'dist/frontend/browser' conforme a pasta gerada pelo Angular
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]