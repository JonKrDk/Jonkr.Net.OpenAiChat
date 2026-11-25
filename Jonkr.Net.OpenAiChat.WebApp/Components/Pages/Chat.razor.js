export function scrollToBottom(element) {
	if (element) {
		element.scrollTop = element.scrollHeight;
	}
}

export function focus(element) {
	if (element && document.activeElement !== element) {
		element.focus();
	}
}

export function enterToSubmit(textarea) {
	textarea.addEventListener("keydown", e => {
		if (e.key === "Enter" && !e.shiftKey && !e.isComposing) {
			e.preventDefault();
			textarea.form?.requestSubmit();
		}
	});
}
