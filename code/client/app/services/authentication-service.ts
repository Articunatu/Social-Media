import api from './api';
import {
    LoginCommand,
    SignUpCommand,
    LogoutCommand,
    RefreshTokenCommand,
    ChangePasswordCommand,
    AuthorizeResponse,
} from '../models/api/authentication-models';

const authUri = '/auth';

const authenticationService = {
    signUp: (command: SignUpCommand) =>
        api.post(`${authUri}/signup`, command),

    login: (command: LoginCommand) =>
        api.post(`${authUri}/login`, command),

    authorize: (token: string) =>
        api.get<AuthorizeResponse>(`${authUri}/authorize`, {
        headers: { Authorization: `Bearer ${token}` },
        }),

    logout: (command: LogoutCommand) =>
        api.post(`${authUri}/logout`, command),

    refreshToken: (command: RefreshTokenCommand) =>
        api.post(`${authUri}/refresh-token`, command),

    changePassword: (command: ChangePasswordCommand) =>
        api.post(`${authUri}/change-password`, command),
};

export default authenticationService;