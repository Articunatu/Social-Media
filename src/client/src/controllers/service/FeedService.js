
class FeedService {
    async fetchFeedPosts() {
      const response = await fetch(URL + 'feed');
      const data = await response.json();
      return data;
    }
  }
  
  export default new FeedService();