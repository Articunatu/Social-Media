import URL from '../Url'; 

class ProfileInfoService {
  async fetchFeedPosts(profileId) {
    const response = await fetch(URL + 'profile/' + profileId);
    const data = await response.json();
    return data;
  }
}

export default new ProfileInfoService();