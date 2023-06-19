
const DEVELOPMENT = 'https://localhost:7284/api/';

const PRODUCTION = 'azure';

const BASE_URL = process.env.NODE_ENV === 'development' ? DEVELOPMENT : PRODUCTION;

export const URL = {
    feed(loggedInUser) {
        return `${BASE_URL}feed/${loggedInUser}`;
    },
    profileInfo(userId) {
        return `${BASE_URL}profile/${userId}`;
    }
}