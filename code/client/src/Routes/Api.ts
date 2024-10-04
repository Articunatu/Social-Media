import axios from "axios";
import { PagedPosts } from "./Models";
import { UUID } from "crypto";

function getBaseApiUrl(version: number): string {
    // Provide the actual implementation
    // For example: return `https://api.example.com/v${version}/`;
    return '';
}

const API = {
    profiles: {
        async login(email: string, password: string) {
            return await axios.post<string, string>(`${getBaseApiUrl(1)}api/profiles/login`, {
                email: email,
                password: password
            });
        }
    },
    quickQuestions: {
        getProfileInfo() {
            // return `${getBaseApiUrl(1)}`;
        },
        getFollowersPaged(id : string) {
            return `${getBaseApiUrl(1)}quickquestion/question/${id}`;
        },
        getFeed() {
            return `${getBaseApiUrl(1)}quickquestion/answer`;
        },
        reportAnswer(answerId : string) {
            return `${getBaseApiUrl(1)}quickquestion/answer/report/${answerId}`;
        }
        },
        
    feed: {
        async get10posts(userId : UUID) : Promise<PagedPosts> {
            return await axios.get<PagedPosts, PagedPosts>(`${getBaseApiUrl(1)}api/profiles/feed/${userId}`);
        },
        react() {
            return `${getBaseApiUrl(1)}reaction/save`;
        },
        vote() {
            return `${getBaseApiUrl(3)}feed/vote`;
        }
    },
}

export default API;