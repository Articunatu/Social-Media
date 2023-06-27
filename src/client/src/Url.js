import config from './Config';

export const URL = {
    signup: config.authentication.signup,
    login: config.authentication.login,
    loggedInId: config.authentication.loggedInId,
    feed: config.feed.getFeed,
    profileInfo: config.profile.getProfileInfo,
};
