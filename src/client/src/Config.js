const DEVELOPMENT = 'https://localhost:44379/api/';
const PRODUCTION = 'azure';

const BASE_URL = process.env.NODE_ENV === 'development' ? DEVELOPMENT : PRODUCTION;

export default {
    authentication: {
        signup: `${BASE_URL}authentication/signup`,
        login: `${BASE_URL}authentication/login`,
        loggedInId: `${BASE_URL}authentication/logged-in-id`,
    },
    feed: {
        getFeed: (loggedInUser) => `${BASE_URL}feed/${loggedInUser}`,
    },
    profile: {
        getProfileInfo: (userId) => `${BASE_URL}profile/${userId}`,
    },
};
