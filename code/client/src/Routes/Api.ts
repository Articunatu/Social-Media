import axios from "axios";

function getBaseApiUrl(version: number): string {
    // Provide the actual implementation
    // For example: return `https://api.example.com/v${version}/`;
    return '';
}

export const API = {
    profiles: {
        login(email: string, password: string) {
            return axios.post(`${getBaseApiUrl(1)}api/profiles/login`, {
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
        react() {
            return `${getBaseApiUrl(1)}reaction/save`;
        },
        vote() {
            return `${getBaseApiUrl(3)}feed/vote`;
        }
    },
}