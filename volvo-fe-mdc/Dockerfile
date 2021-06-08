FROM node:14.15.4-alpine 
RUN apk add yarn
WORKDIR /app
COPY package.json .
RUN npm install @types/qs
RUN yarn
COPY . .
EXPOSE 3000
##CMD npm run start
CMD yarn start-prod

