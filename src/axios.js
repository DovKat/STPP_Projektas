import axios from 'axios';
import config from './config';

// Create an instance of axios with the base URL configured
const axiosInstance = axios.create({
  baseURL: config.apiBaseUrl,
  headers: {
    'Content-Type': 'application/json',
  },
});

axios.defaults.withCredentials = true;

export default axiosInstance;
