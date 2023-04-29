
const DEVELOPMENT = 'https://localhost:7284/api/';

const PRODUCTION = 'azure';

const URL = process.env.NODE_ENV === 'development' ? DEVELOPMENT : PRODUCTION;

export default URL;