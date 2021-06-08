let BASE_URL = "http://localhost:5000/api/ecash"

if (process.env.REACT_APP_ENV==="development"){
  BASE_URL = "http://volvo.brq.com:5004/api/ecash"
}

if (process.env.REACT_APP_ENV==="production"){
  BASE_URL = "https://volvo.brq.com/api/ecash"
}

export {BASE_URL}