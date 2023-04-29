
class MessageService {
    async fetchFeedPosts() {
      const response = await fetch('https://localhost:7284/api/Message/read-all');
      const data = await response.json();
      return data;
    }
  }
  
  export default new MessageService();