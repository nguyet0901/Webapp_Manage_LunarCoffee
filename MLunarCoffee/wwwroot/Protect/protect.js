"use strict";

(() => {
	const main = document.getElementsByTagName("main").item(0);
	/**@type {(isOpen:boolean) => void}*/
	let renderIsOpen = (isOpen) => { };
	{

		const status = document.createElement("div");
		status.style.display = "grid";
		{
			//const wrap = document.createElement("div");
			//{
			//	const label = document.createElement("label");
			//	label.textContent = "devtoolsDetector.isOpen: ";
			//	wrap.appendChild(label);
			//} {
			//	const isOpen = document.createElement("output");
			//	const f = document.createTextNode("false");
			//	const t = document.createElement("span");
			//	{
			//		const emoji = (/**@type {string}*/text) => {
			//			const el = document.createElement("div");
			//			el.classList.add("dancing-emoji");
			//			el.textContent = text;
			//			return el;
			//		};
			//		t.appendChild(emoji("🕺"));
			//		t.appendChild(document.createTextNode("true"));
			//		t.appendChild(emoji("💃"));
			//	}
			//	renderIsOpen = (val) => {
			//		isOpen.replaceChild((val ? t : f), isOpen.firstChild);
			//	};
			//	isOpen.appendChild(f);
			//	wrap.appendChild(isOpen);
			//}
			//status.appendChild(wrap);
		} {
			//const label = document.createElement("label");
			//label.textContent = "devtoolsDetector.paused";
			//const pause = document.createElement("input");
			//pause.type = "checkbox";
			//pause.onchange = (ev) => {devtoolsDetector.paused = pause.checked;};
			//label.appendChild(pause);
			//status.appendChild(label);
		}
		main?.append(status);
	}

	const form = document.createElement("fieldset");
	Object.assign(form.style, {
		contain: "content", position: "relative",
		display: "flex", flexFlow: "column",
	});
	{
		//const legend = document.createElement("legend");
		//legend.textContent = "devtoolsDetector.config";
		//form.appendChild(legend);
	}
	const fields = {
		pollingIntervalSeconds: {type: "number", default: 1.0, min: 0},
		maxMillisBeforeAckWhenClosed: {type: "number", default: 100, min: 0},
		moreAnnoyingDebuggerStatements: {type: "number", default: 0, min: 0},

		onDetectOpen: {type: "eval-js"},
		onDetectClose: {type: "eval-js"},

		startup: {
			type: "select", options: ["manual", "asap", "domContentLoaded"], default: "asap",
		},
		onCheckOpennessWhilePaused: {
			type: "select", options: ["returnStaleValue", "throw"], default: "returnStaleValue",
		},
	};
	Object.keys(fields).forEach((field) => {

		// @ts-expect-error
		const desc = fields[field];
		const label = document.createElement("label");
		label.textContent = field + ": ";
		if (desc.type === "eval-js") {
			const area = document.createElement("textarea");
			area.style.display = "grid";
			//area.placeholder = "< something to eval() >";
			area.onchange = (ev) => {
				devtoolsDetector.config[field] = () => {
					renderIsOpen(devtoolsDetector.isOpen);
					try {
						eval(area.value);
					} catch (err) {
						alert(err.message + "\n\n " + err.stack);
					}
				};
			};
			area.dispatchEvent(new Event("change"));
			label.appendChild(area);
		}
		//form.appendChild(label);
	});
	main?.appendChild(form);
})();

