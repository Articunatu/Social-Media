import type { SignUpCommand } from "../../../models/api/authentication-models";

export type SignUpFormValues = Omit<SignUpCommand, "firstName" | "lastName"> & {
    firstName: string;
    lastName: string;
    confirmPassword: string;
};

export type SignUpField = keyof SignUpFormValues;
export type SignUpErrors = Partial<Record<SignUpField, string>>;

const tagPattern = /^[a-z0-9_-]{2,20}$/;
const namePattern = /^[\p{L}'][ \p{L}'-]*[\p{L}]$/u;
const emailPattern = /^[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+$/;
const passwordPattern = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*\-+]).+$/;

export const signUpLimits = {
    tagMaxLength: 20,
    firstNameMaxLength: 25,
    lastNameMaxLength: 40,
    emailMaxLength: 100,
    passwordMinLength: 8,
    passwordMaxLength: 99,
};

export function validateSignUp(values: SignUpFormValues): SignUpErrors {
    const errors: SignUpErrors = {};
    const tag = values.tag.trim();
    const firstName = values.firstName?.trim() ?? "";
    const lastName = values.lastName?.trim() ?? "";
    const email = values.email.trim();

    if (!tag) {
        errors.tag = "Tag is required.";
    } else if (!tagPattern.test(tag)) {
        errors.tag = "Use 2-20 lowercase letters, numbers, underscores, or hyphens.";
    }

    if (!firstName) {
        errors.firstName = "First name is required.";
    } else if (firstName.length > signUpLimits.firstNameMaxLength || !namePattern.test(firstName)) {
        errors.firstName = "Use letters, spaces, apostrophes, or hyphens, ending with a letter.";
    }

    if (!lastName) {
        errors.lastName = "Last name is required.";
    } else if (lastName.length > signUpLimits.lastNameMaxLength || !namePattern.test(lastName)) {
        errors.lastName = "Use letters, spaces, apostrophes, or hyphens, ending with a letter.";
    }

    if (!email) {
        errors.email = "Email is required.";
    } else if (email.length > signUpLimits.emailMaxLength || !emailPattern.test(email)) {
        errors.email = "Enter a valid email address up to 100 characters.";
    }

    if (!values.password) {
        errors.password = "Password is required.";
    } else if (
        values.password.length < signUpLimits.passwordMinLength ||
        values.password.length > signUpLimits.passwordMaxLength ||
        !passwordPattern.test(values.password)
    ) {
        errors.password = "Use 8-99 characters with uppercase, lowercase, number, and special character.";
    }

    if (!values.confirmPassword) {
        errors.confirmPassword = "Confirm your password.";
    } else if (values.confirmPassword !== values.password) {
        errors.confirmPassword = "Passwords must match.";
    }

    return errors;
}

export function hasSignUpErrors(errors: SignUpErrors) {
    return Object.keys(errors).length > 0;
}

export function toSignUpCommand(values: SignUpFormValues): SignUpCommand {
    return {
        tag: values.tag.trim(),
        email: values.email.trim(),
        firstName: values.firstName?.trim() ?? "",
        lastName: values.lastName?.trim() ?? "",
        password: values.password,
    };
}
