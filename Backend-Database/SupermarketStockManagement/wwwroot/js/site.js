document.addEventListener("DOMContentLoaded", function () {
    const htmlElement = document.documentElement;

    const themeToggle =
        document.getElementById("themeToggle");

    const sidebar =
        document.getElementById("sidebar");

    const sidebarToggle =
        document.getElementById("sidebarToggle");

    const sidebarOverlay =
        document.getElementById("sidebarOverlay");


    // =====================================
    // LOAD SAVED THEME
    // =====================================

    const savedTheme =
        localStorage.getItem("stockflow-theme");

    if (savedTheme) {
        htmlElement.setAttribute(
            "data-theme",
            savedTheme
        );
    } else {
        // Use the device theme when the user
        // has not selected a theme before
        const deviceUsesDarkMode =
            window.matchMedia(
                "(prefers-color-scheme: dark)"
            ).matches;

        htmlElement.setAttribute(
            "data-theme",
            deviceUsesDarkMode
                ? "dark"
                : "light"
        );
    }


    // =====================================
    // LIGHT / DARK MODE BUTTON
    // =====================================

    if (themeToggle) {
        themeToggle.addEventListener(
            "click",
            function () {
                const currentTheme =
                    htmlElement.getAttribute(
                        "data-theme"
                    );

                const newTheme =
                    currentTheme === "dark"
                        ? "light"
                        : "dark";

                htmlElement.setAttribute(
                    "data-theme",
                    newTheme
                );

                localStorage.setItem(
                    "stockflow-theme",
                    newTheme
                );

                // Notify charts that the theme changed
                window.dispatchEvent(
                    new CustomEvent(
                        "stockflowThemeChanged",
                        {
                            detail: {
                                theme: newTheme
                            }
                        }
                    )
                );
            }
        );
    }


    // =====================================
    // OPEN MOBILE SIDEBAR
    // =====================================

    function openSidebar() {
        if (sidebar) {
            sidebar.classList.add(
                "sidebar-open"
            );
        }

        if (sidebarOverlay) {
            sidebarOverlay.classList.add(
                "overlay-visible"
            );
        }

        document.body.style.overflow =
            "hidden";
    }


    // =====================================
    // CLOSE MOBILE SIDEBAR
    // =====================================

    function closeSidebar() {
        if (sidebar) {
            sidebar.classList.remove(
                "sidebar-open"
            );
        }

        if (sidebarOverlay) {
            sidebarOverlay.classList.remove(
                "overlay-visible"
            );
        }

        document.body.style.overflow = "";
    }


    if (sidebarToggle) {
        sidebarToggle.addEventListener(
            "click",
            openSidebar
        );
    }

    if (sidebarOverlay) {
        sidebarOverlay.addEventListener(
            "click",
            closeSidebar
        );
    }


    // Close sidebar after selecting a page
    const sidebarLinks =
        document.querySelectorAll(
            ".sidebar-link"
        );

    sidebarLinks.forEach(function (link) {
        link.addEventListener(
            "click",
            function () {
                if (
                    window.innerWidth <= 991
                ) {
                    closeSidebar();
                }
            }
        );
    });


    // Close sidebar when Escape is pressed
    document.addEventListener(
        "keydown",
        function (event) {
            if (event.key === "Escape") {
                closeSidebar();
            }
        }
    );


    // Reset mobile sidebar when resizing
    window.addEventListener(
        "resize",
        function () {
            if (window.innerWidth > 991) {
                closeSidebar();
            }
        }
    );


    // =====================================
    // ACTIVE SIDEBAR LINK
    // =====================================

    const currentPath =
        window.location.pathname.toLowerCase();

    sidebarLinks.forEach(function (link) {
        const linkPath =
            new URL(
                link.href,
                window.location.origin
            ).pathname.toLowerCase();

        link.classList.remove("active");

        const isHomePage =
            linkPath === "/" &&
            currentPath === "/";

        const isControllerPage =
            linkPath !== "/" &&
            currentPath.startsWith(linkPath);

        if (
            isHomePage ||
            isControllerPage
        ) {
            link.classList.add("active");
        }
    });


    // =====================================
    // TABLE ROW ANIMATION
    // =====================================

    const tableRows =
        document.querySelectorAll(
            ".table tbody tr"
        );

    tableRows.forEach(
        function (row, index) {
            row.style.opacity = "0";
            row.style.transform =
                "translateY(8px)";

            setTimeout(
                function () {
                    row.style.transition =
                        "opacity 0.3s ease, " +
                        "transform 0.3s ease";

                    row.style.opacity = "1";
                    row.style.transform =
                        "translateY(0)";
                },
                index * 45
            );
        }
    );
});