
class FeedService {
  async fetchFeedPosts() {
    try {
      const response = await fetch(URL + 'feed');
      const data = await response.json();
      return data;
    } catch (error) {
      // Handle the error appropriately (e.g., throw a custom error, log the error, or return a default value)
      throw new Error('Failed to fetch feed posts.');
    }
  }
}

export default new FeedService();